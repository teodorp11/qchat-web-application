import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-join-room',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './join-room.html',
  styleUrl: './join-room.css',
})
export class JoinRoom implements OnInit {
  joinRoomForm!: FormGroup;
  fb = inject(FormBuilder);
  router = inject(Router);

  ngOnInit(): void {
    this.joinRoomForm = this.fb.group({
      user: ['', Validators.required],
      room: ['', Validators.required],
    });
  }

  joinRoom() {
    console.log(this.joinRoomForm.value);
    this.router.navigate(['chat']);
  }
}
