import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetPlanListComponent } from './budget-plan-list.component';

describe('BudgetPlanListComponent', () => {
  let component: BudgetPlanListComponent;
  let fixture: ComponentFixture<BudgetPlanListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BudgetPlanListComponent]
    });
    fixture = TestBed.createComponent(BudgetPlanListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
