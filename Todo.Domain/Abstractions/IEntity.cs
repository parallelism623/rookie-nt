﻿namespace Todo.Domain.Abstractions;
public interface IEntity<TKey>
{
    public TKey Id { get; set; }
}
    