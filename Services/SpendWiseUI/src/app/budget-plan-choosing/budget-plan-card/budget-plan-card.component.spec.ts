import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetPlanCardComponent } from './budget-plan-card.component';

describe('BudgetPlanCardComponent', () => {
  let component: BudgetPlanCardComponent;
  let fixture: ComponentFixture<BudgetPlanCardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BudgetPlanCardComponent]
    });
    fixture = TestBed.createComponent(BudgetPlanCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
