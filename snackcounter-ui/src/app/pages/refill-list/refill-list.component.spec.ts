import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RefillListComponent } from './refill-list.component';

describe('RefillListComponent', () => {
  let component: RefillListComponent;
  let fixture: ComponentFixture<RefillListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RefillListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RefillListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
