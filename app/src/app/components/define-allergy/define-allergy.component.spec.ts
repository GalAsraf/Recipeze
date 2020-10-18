import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefineAllergyComponent } from './define-allergy.component';

describe('DefineAllergyComponent', () => {
  let component: DefineAllergyComponent;
  let fixture: ComponentFixture<DefineAllergyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefineAllergyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefineAllergyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
