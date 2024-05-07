import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewKeypointComponent } from './new-keypoint.component';

describe('NewKeypointComponent', () => {
  let component: NewKeypointComponent;
  let fixture: ComponentFixture<NewKeypointComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewKeypointComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NewKeypointComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
