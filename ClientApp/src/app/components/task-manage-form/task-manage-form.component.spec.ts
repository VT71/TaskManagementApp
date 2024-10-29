import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskManageFormComponent } from './task-manage-form.component';

describe('TaskManageFormComponent', () => {
  let component: TaskManageFormComponent;
  let fixture: ComponentFixture<TaskManageFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskManageFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TaskManageFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
