#include <stdint.h>
#include <stddef.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include <stdio.h>
#pragma pack(1)

/* Renamed global counter for sound segment IDs */
int8_t globalID = 0;
int16_t* data;
struct wav_header {
    char chunk_id[4];         // "RIFF"
    uint32_t chunk_size;
    char format[4];           // "WAVE"
    char subchunk1_id[4];     // "fmt "
    uint32_t subchunk1_size;
    uint16_t audio_format;
    uint16_t num_channels;
    uint32_t sample_rate;
    uint32_t byte_rate;
    uint16_t block_align;
    uint16_t bits_per_sample;
    char subchunk2_id[4];     // "data"
    uint32_t subchunk2_size;
};

/* Global variable renamed for clarity */
int graph_size = 0;

struct AdjListNode {
    int16_t ID;           // Node field (sound segment ID)
    int16_t IDN;
    int16_t data;
    int16_t sizeChild;
    struct AdjListNode** next;
};

struct node {
    struct AdjListNode** graph;
};
struct node a = { .graph = NULL };

/* Use a different parameter name to avoid shadowing */
void create_graph(size_t new_size) {
    static size_t old_size = 0;
    if(a.graph == NULL){
        a.graph = calloc(new_size, sizeof(struct AdjListNode*));
        if(a.graph == NULL) {
            fprintf(stderr, "Error: calloc failed in create_graph\n");
            exit(EXIT_FAILURE);
        }
        old_size = new_size;
    } else {
        a.graph = realloc(a.graph, new_size * sizeof(struct AdjListNode*));
        if(a.graph == NULL) {
            fprintf(stderr, "Error: realloc failed in create_graph\n");
            exit(EXIT_FAILURE);
        }
        if(new_size > old_size) {
            memset(a.graph + old_size, 0, (new_size - old_size) * sizeof(struct AdjListNode*));
        }
        old_size = new_size;
    }
    graph_size = new_size;
}

void add_edge(struct AdjListNode* graph, int16_t childID, int16_t childIDN) {
    graph->next = realloc(graph->next, (graph->sizeChild + 1) * sizeof(struct AdjListNode*));
    if(graph->next == NULL) {
        fprintf(stderr, "Error: realloc failed in add_edge\n");
        exit(EXIT_FAILURE);
    }
    struct AdjListNode* newNode = malloc(sizeof(struct AdjListNode));
    if(newNode == NULL) {
        fprintf(stderr, "Error: malloc failed in add_edge\n");
        exit(EXIT_FAILURE);
    }
    newNode->ID = childID;
    newNode->IDN = childIDN;
    newNode->data = 0;
    newNode->sizeChild = 0;
    newNode->next = NULL;
    graph->next[graph->sizeChild] = newNode;
    graph->sizeChild++;
}

void add_edge_dfs(struct AdjListNode* graph, int child_ID, int child_IDN, int targetID, int targetIDN) {
    if (graph == NULL) return;
    if (graph->ID == targetID && graph->IDN == targetIDN) {
        add_edge(graph, child_ID, child_IDN);
        return;
    }
    if (graph->sizeChild == 0) return;
    for (int i = 0; i < graph->sizeChild; i++) {
        add_edge_dfs(graph->next[i], child_ID, child_IDN, targetID, targetIDN);
    }
}

void change_edge_data(struct AdjListNode* graph, int child_ID, int child_IDN, int new_child_data) {
    if(graph == NULL) return;
    if(graph->ID == child_ID && graph->IDN == child_IDN) {
        graph->data = new_child_data;
        return;
    }
    if(graph->sizeChild == 0) return;
    for (int i = 0; i < graph->sizeChild; i++) {
        change_edge_data(graph->next[i], child_ID, child_IDN, new_child_data);
    }
}

void change_edge_index(struct AdjListNode* graph, int child_ID, int new_index) {
    if(graph == NULL) return;
    if(graph->ID == child_ID) {
        graph->IDN = new_index;
        return;
    }
    if(graph->sizeChild == 0) return;
    for (int i = 0; i < graph->sizeChild; i++) {
        change_edge_index(graph->next[i], child_ID, new_index);
    }
}

void get_data_from_graph(struct AdjListNode* graph, int child_ID) {
    if(graph == NULL) return;
    if(graph->ID == child_ID)
    {
        
        data[graph->IDN] = graph->data;
        printf("%d ", data[graph->IDN]);
    }

    
    if(graph->sizeChild == 0) return;
    for (int i = 0; i < graph->sizeChild; i++) {
        get_data_from_graph(graph->next[i], child_ID);
    }
}

void delete_edge_IDN_on_delete(struct AdjListNode* graph, int child_ID, int l, int r) {
    if (graph == NULL) return;
    if(graph->ID != child_ID) return;
    if(graph->sizeChild == 0) return;
    int16_t* list_del_id = NULL;
    int cnt = 0;
    for (int i = 0; i < graph->sizeChild; i++) {
        if(graph->next[i]->ID == child_ID && 
           graph->next[i]->IDN >= l && 
           graph->next[i]->IDN <= r) {
            cnt++;
            int16_t* temp = realloc(list_del_id, cnt * sizeof(int16_t));
            if (temp == NULL) {
                free(list_del_id);
                return;
            }
            list_del_id = temp;
            list_del_id[cnt - 1] = i;
        } else {
            delete_edge_IDN_on_delete(graph->next[i], child_ID, l, r);
        }
    }
    int ct = 0;
    for (int j = 0; j < graph->sizeChild; j++) {
        if (ct < cnt && j == list_del_id[ct]) {
            ct++;
        } else {
            graph->next[j - ct] = graph->next[j];
        }
    }
    graph->sizeChild -= cnt;
    free(list_del_id);
}

struct sound_seg {
    size_t num_samples;
    size_t ID;
};

void wav_load(const char* fname, int16_t* dest) {
    FILE* file = fopen(fname, "rb");
    if (!file) return;
    char chunk_id[4];
    uint32_t chunk_size;
    fseek(file, 12, SEEK_SET); // skip RIFF + WAVE
    while (fread(chunk_id, 1, 4, file) == 4) {
        fread(&chunk_size, 4, 1, file);
        if (memcmp(chunk_id, "data", 4) == 0) {
            fread(dest, sizeof(int16_t), chunk_size / 2, file);
            break;
        } else {
            fseek(file, chunk_size, SEEK_CUR);
        }
    }
    fclose(file);
}

void wav_save(const char* fname, const int16_t* src, size_t len) {
    FILE* file = fopen(fname, "wb");
    if (!file) return;
    struct wav_header header;
    memcpy(header.chunk_id, "RIFF", 4);
    memcpy(header.format, "WAVE", 4);
    memcpy(header.subchunk1_id, "fmt ", 4);
    memcpy(header.subchunk2_id, "data", 4);
    header.subchunk1_size = 16;
    header.audio_format = 1;
    header.num_channels = 1;
    header.sample_rate = 8000;
    header.bits_per_sample = 16;
    header.byte_rate = header.sample_rate * header.num_channels * header.bits_per_sample / 8;
    header.block_align = header.num_channels * header.bits_per_sample / 8;
    header.subchunk2_size = len * sizeof(int16_t);
    header.chunk_size = 36 + header.subchunk2_size;
    fwrite(&header, sizeof(struct wav_header), 1, file);
    fwrite(src, sizeof(int16_t), len, file);
    fclose(file);
}

struct sound_seg* tr_init() {
    struct sound_seg* seg = malloc(sizeof(struct sound_seg));
    if (!seg) return NULL;
    seg->num_samples = 0;
    seg->ID = ++globalID;
    return seg;
}

void getdata(int16_t ID) {
    if(data == NULL) {
        fprintf(stderr, "Error: malloc failed in getdata\n");
        exit(EXIT_FAILURE);
    }
    
  
    for (int i = 0; i < graph_size; i++) {
        if (a.graph[i] != NULL){
            
            get_data_from_graph(a.graph[i], ID);
        }
    }

}

void destroy_graph(struct AdjListNode* graph) {
    if (graph == NULL) return;
    for (int i = 0; i < graph->sizeChild; i++) {
        destroy_graph(graph->next[i]);
    }
    free(graph->next);
    free(graph);
}

void tr_destroy(struct sound_seg* track) {
    if (!track) return;
    for (int i = 0; i < graph_size; i++) {
        if (a.graph[i] && a.graph[i]->ID == track->ID) {
            destroy_graph(a.graph[i]);
            a.graph[i] = NULL;
        }
    }
    free(track);
}

size_t tr_length(struct sound_seg* track) {
    if (!track) return 0;
    return track->num_samples;
}
void make_data(int16_t size, int16_t ID)
{
    if(data)
    {
        free(data);
    }
    printf("%d ", size);
    data = realloc(data, sizeof(int16_t) * size);
    for(int i = 0; i < 3; i++) printf("%d ", data[i]);
    getdata(ID);
 
}
void tr_read(struct sound_seg* track, int16_t* dest, size_t pos, size_t len) {
    if (!track || !dest || pos >= track->num_samples) return;
    size_t copy_len = len;
    printf("%d ", track->num_samples);
    make_data(track->num_samples, track->ID);

    if (pos + len > track->num_samples) {
        copy_len = track->num_samples - pos;
    }

    memcpy(dest, data + pos, copy_len * sizeof(int16_t));

}
void update_date_dfs(struct AdjListNode* graph, const int16_t* src, int16_t ID, int l, int r)
{
    if(graph->ID == ID && graph->IDN >= l && graph->IDN <= r)
    {
        graph->data = src[graph->IDN - l];
    }
    if(graph->sizeChild == 0)
    {
        return;
    }
    for(int i = 0; i < graph->sizeChild; i++)
    {
        update_date_dfs(graph->next[i], src, ID, l, r);
    }
}
void tr_write(struct sound_seg* track, const int16_t* src, size_t pos, size_t len) {
    if (!track || !src) return;

    int16_t oldSize = graph_size;
     for(int i = 0; i < graph_size; i++)
    {
        update_date_dfs(a.graph[i], src, track->ID, pos, pos + len - 1);
    }
    if (len + pos > track->num_samples) {
        graph_size += (len + pos - track->num_samples);
    
        track->num_samples = pos + len;
    }
 
    create_graph(graph_size);
    printf("graph_size = %d\n", graph_size);
    for (size_t i = oldSize; i < graph_size; i++) {
        printf("Before: a.graph[%zu] = %p\n", i, (void*)a.graph[i]);
        if (a.graph[i] == NULL) {
            a.graph[i] = malloc(sizeof(struct AdjListNode));
            if (a.graph[i] == NULL) {
                printf("Error: Memory allocation failed for graph[%zu].\n", i);
                return;
            }
            a.graph[i]->ID = track->ID;
            a.graph[i]->IDN = i;
            a.graph[i]->sizeChild = 0;
            a.graph[i]->next = NULL;
        }

        a.graph[i]->data = src[i - oldSize];
        printf("track->ID = %zu\n", track->ID);
        printf("a.graph[%zu]->data = %d\n", i, src[i - oldSize]);
        printf("a.graph[%zu]->ID = %d\n", i, a.graph[i]->ID);
        printf("a.graph[%zu]->IDN = %d\n", i, a.graph[i]->IDN);
    }
    
}

bool can_delete_graph(struct AdjListNode* graph, int child_ID, int child_st, int child_en) {
    if(graph->ID == child_ID && graph->IDN >= child_st && graph->IDN <= child_en && graph->sizeChild > 0) {
        return false;
    }
    if(graph->sizeChild == 0) {
        return true;
    }
    for (size_t i = 0; i < graph->sizeChild; i++) {
        if(!can_delete_graph(graph->next[i], child_ID, child_st, child_en))
            return false;
    }
    return true;
}

bool can_delete(int child_ID, int st, int en) {
    for (int i = 0; i < graph_size; i++) {
        if(a.graph[i] && !can_delete_graph(a.graph[i], child_ID, st, en))
            return false;
    }
    return true;
}

void tr_delete_range_node(struct sound_seg* track, size_t l, size_t r) {
    for (int i = 0; i < graph_size; i++) {
        if(a.graph[i])
            delete_edge_IDN_on_delete(a.graph[i], track->ID, l, r);
    }
}

bool tr_delete_range(struct sound_seg* track, size_t pos, size_t len) {
    if (!track || pos >= track->num_samples || len == 0)
        return true;
    if (!can_delete(track->ID, pos, pos + len - 1))
        return false;
    tr_delete_range_node(track, pos, pos + len - 1);
    track->num_samples -= len;
    return true;
}

char* tr_identify(const struct sound_seg* target, const struct sound_seg* ad) {
     make_data(target->num_samples, target->ID);
     int16_t* targetData = data;
     make_data(ad->num_samples, ad->ID);
    int16_t* adData = data;
    if (!target || !ad || !targetData || !adData ||
        ad->num_samples == 0 || target->num_samples < ad->num_samples) {
        char* empty = malloc(1);
        if (empty) empty[0] = '\0';
        return empty;
    }
    double ref_autocorr = 0.0;
    for (size_t i = 0; i < ad->num_samples; ++i) {
        ref_autocorr += adData[i] * adData[i];
    }
    char* result = malloc(1);
    size_t result_size = 1;
    result[0] = '\0';
    for (size_t i = 0; i <= target->num_samples - ad->num_samples; ++i) {
        double cross_corr = 0.0;
        for (size_t j = 0; j < ad->num_samples; ++j) {
            cross_corr += targetData[i + j] * adData[j];
        }
        double similarity = cross_corr / ref_autocorr;
        if (similarity >= 0.95) {
            char buffer[64];
            int start = (int)i;
            int end = (int)(i + ad->num_samples - 1);
            snprintf(buffer, sizeof(buffer), "%d,%d\n", start, end);
            size_t buffer_len = strlen(buffer);
            char* temp = realloc(result, result_size + buffer_len);
            if (!temp) break;
            result = temp;
            memcpy(result + result_size - 1, buffer, buffer_len + 1);
            result_size += buffer_len;
            i += ad->num_samples - 1;
        }
    }
    if (result_size > 1 && result[result_size - 2] == '\n') {
        result[result_size - 2] = '\0';
    }
    free(targetData);
    free(adData);
    return result;
}

void insert_node_into_graph(struct AdjListNode* graph, int destpos, int srcpos, int len, int IDSrc, int IDDest) {
    if (graph == NULL)
        return;
    if (graph->ID == IDSrc && graph->IDN >= srcpos && graph->IDN <= srcpos + len - 1) {
        add_edge(graph, IDDest, destpos + (graph->IDN - srcpos));
    }
    for (int i = 0; i < graph->sizeChild; i++) {
        if (graph->next[i]->ID == IDSrc) {
            insert_node_into_graph(graph->next[i], destpos, srcpos, len, IDSrc, IDDest);
        }
    }
}

void update_idn_insert(struct AdjListNode* graph, int nodeID, int l, int len) {
    if (graph == NULL) return;
    if (graph->ID != nodeID) return;
    if (graph->IDN >= l) {
        graph->IDN += len;
    }
    for (int i = 0; i < graph->sizeChild; i++) {
        update_idn_insert(graph->next[i], nodeID, l, len);
    }
}

void tr_insert(struct sound_seg* src_track, struct sound_seg* dest_track, size_t destpos, size_t srcpos, size_t len) {
    for (int i = 0; i < graph_size; i++) {
        if(a.graph[i])
            update_idn_insert(a.graph[i], dest_track->ID, destpos, len);
    }
    for (int i = 0; i < graph_size; i++) {
        if(a.graph[i])
            insert_node_into_graph(a.graph[i], destpos, srcpos, len, src_track->ID, dest_track->ID);
    }
}


int main() {
struct sound_seg* s0 = tr_init();
    tr_write(s0, ((int16_t[]){17,3}), 0, 2);
    struct sound_seg* s1 = tr_init();
    tr_write(s1, ((int16_t[]){-3,11,-18,14,9,-17,6,-8,2,0,-14,-13,18,14,20}), 0, 15);
    struct sound_seg* s2 = tr_init();
    tr_write(s2, ((int16_t[]){4,5,11}), 0, 3);
    struct sound_seg* s3 = tr_init();
    tr_write(s3, ((int16_t[]){13,0,-15}), 0, 3);
    struct sound_seg* s4 = tr_init();
    tr_write(s4, ((int16_t[]){}), 0, 0);
    int16_t FAILING_READ[3];
    tr_read(s3, FAILING_READ, 0, 3);
    //expected [ 13   0 -15], actual [0 0 0]!
printf("Mang doc tu file: ");
    for (int i = 0; i < 3; i++) {
        printf("%d ", FAILING_READ[i]);
    }
    printf("\n");
    tr_destroy(s0);
    tr_destroy(s1);
    tr_destroy(s2);
    tr_destroy(s3);
    tr_destroy(s4);
}
