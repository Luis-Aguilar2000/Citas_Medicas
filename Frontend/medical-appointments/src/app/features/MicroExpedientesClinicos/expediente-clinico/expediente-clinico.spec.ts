import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpedienteClinico } from './expediente-clinico';

describe('ExpedienteClinico', () => {
  let component: ExpedienteClinico;
  let fixture: ComponentFixture<ExpedienteClinico>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpedienteClinico]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpedienteClinico);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
