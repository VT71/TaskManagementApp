/**
 * Interface representing a single ToDoTask.
 */
export interface ToDoTask {
    id: number;
    title: string;
    description: string | null;
    dueDate: Date;
    completed: boolean;
}
