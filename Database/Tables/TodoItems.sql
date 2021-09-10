CREATE TABLE [dbo].[TodoItems]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [todo_list_id] INT NOT NULL,
    CONSTRAINT [FK_ToDoItems_ToTodoLists] FOREIGN KEY ([todo_list_id]) REFERENCES [ToDoLists]([id]), 
    [is_sub_item] BIT NOT NULL DEFAULT 0, 
    [parent_item] int NULL, 
	CONSTRAINT [FK_ToDoItems_ToPrimaryKey] FOREIGN KEY ([parent_item]) REFERENCES [ToDoItems]([id]), 
    [description] NVARCHAR(100) NULL, 
    [is_completed] BIT NOT NULL DEFAULT 0
)
