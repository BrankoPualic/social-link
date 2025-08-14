import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BaseFormComponent } from '../../../../shared/base/base-form';
import { ILoginModel } from '../../models/login-model';
import { InputText } from '../../../../shared/components/forms/input-text';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, InputText, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login extends BaseFormComponent<ILoginModel> implements OnInit {
  constructor(
    fb: FormBuilder
  ) {
    super(fb);
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  override initializeForm(): void {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  override submit(): void {
    throw new Error('Method not implemented.');
  }
}
