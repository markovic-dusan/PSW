import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlockedUSersComponent } from './blocked-users.component';

describe('BlockedUSersComponent', () => {
  let component: BlockedUSersComponent;
  let fixture: ComponentFixture<BlockedUSersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BlockedUSersComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BlockedUSersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
