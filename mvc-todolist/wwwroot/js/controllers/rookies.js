
function getFilterValues() {
    const searchText = document.querySelector('.filter-container input[type="text"]').value.trim();
    const ageFrom = $('.age-input .from-age').val();
    const ageTo = $('.age-input .to-age').val();

    const genders = [];
    document.querySelectorAll('.filter-container input[type="checkbox"]:checked').forEach(checkbox => {
        genders.push(checkbox.value);
    });
    const selectedGender = $('.filter-container .form-check-input[type="radio"]:checked').val() || null;
    const birthYearFrom = $('.birthyear-input .from-birthyear').val();
    const birthYearTo = $('.birthyear-input .to-birthyear').val();
    var filter = "";
    if (!!ageFrom) {
        filter += `Age>=${ageFrom};`;
    }
    if (!!ageTo) {
        filter += `Age<=${ageTo};`;
    }
    if (!!selectedGender) {
        filter += `Gender==${selectedGender};`
    }
    if (!!birthYearFrom) {
        filter += `BirthYear>=${birthYearFrom};`
    }
    if (!!birthYearTo) {
        filter += `BirthYear<=${birthYearTo};`
    }
    return {
        search: searchText,
        filter: filter,
    };
}
function buildQueryUrl(params) {
    const url = new URL(window.location.origin + '/NashTech/Rookies/Index');
    url.searchParams.set('filter', params.filter);
    url.searchParams.set('search', params.search);
    url.searchParams.set('logicfilter', "and");
    return url.toString();
}

document.querySelector('.filter-button').addEventListener('click', function (event) {
    getListRookies(event);
});


function getListRookies(event) {
    event.preventDefault();
    const filterValues = getFilterValues();
    const queryUrl = buildQueryUrl(filterValues);
    window.location.href = queryUrl;
}

function handleRemovePerson(id) {

    fetch(`${window.location.origin}/NashTech/Rookies/Remove/${id}`, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' }
    }).catch(e => {
            alert(e.message);
        });
}
function handleExport(data) {
    console.log(data);
    fetch(`${window.location.origin}/NashTech/Rookies/ExportToExcel`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ personViewModels: data })
    })
        .then(response => response.blob())
        .then(blob => {
            let url = window.URL.createObjectURL(blob);
            let a = document.createElement('a');
            a.href = url;
            a.download = "rookies-list.xlsx";
            document.body.appendChild(a);
            a.click();
            a.remove();
        });
}
//$(document).ready(function (event) {
//    $('.column-toggle-btn').on('click', function (event) {
//        event.stopPropagation(); 
//        $('.column-list').toggle();
//    });

//    $(document).on('click', function () {
//        $('.column-list').hide();
//    });

    
//    $('.column-list').on('click', function (event) {
//        event.stopPropagation();
//    });
//    getListRookies(event);
//});
//function handleOnColumnListSelected(list,name, id) {
//    if (!!list.find(it => it === name)) {
//        list.slice(id, 1);
//    }
//    else {
//        list.splice(i, 0, name);
//    }
//}