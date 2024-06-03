import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MaliciousUsersComponent } from './malicious-users.component';

describe('MaliciousUsersComponent', () => {
  let component: MaliciousUsersComponent;
  let fixture: ComponentFixture<MaliciousUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MaliciousUsersComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MaliciousUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
