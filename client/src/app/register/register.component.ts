import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  maxDate: Date;
  label: any;
  validationErrors: string[]=[];
  constructor(private accountService: AccountService, private toastr: ToastrService,
    private fb:FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializrForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializrForm(){
    this.registerForm = this.fb.group({
      gender : ['male'],
      username : ['',Validators.required],
      knownAs : ['',Validators.required],
      dateOfBirth : ['',Validators.required],
      city : ['',Validators.required],
      country : ['',Validators.required],
      password : ['',[Validators.required,Validators.minLength(4)]],
      confirmPassword : ['',[Validators.required, this.matchPassword.bind(this)]]
    });
    this.registerForm.controls.password.valueChanges.subscribe(()=>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }
  matchPassword(control: AbstractControl): { [key: string]: boolean } | null {
    const password = this.registerForm?.get('password')?.value;
    const confirmPassword = control.value;

    if (password !== confirmPassword) {
      return { isMatching : true };
    }

    return null;
  }

  // matchValues(matchTo: string): ValidatorFn{
  //   return (control: AbstractControl)=>{
  //     return control?.value === control?.parent.controls[matchTo].value 
  //     ? null : {isMatching:true}
  //   }
  // }

  register(){
    this.accountService.register(this.registerForm.value).subscribe(response => {
      //console.log(response);
      this.router.navigateByUrl('/members');
      this.cancel();
    }, error=>{
      this.validationErrors = error;
    });
  }

  cancel(){
   this.cancelRegister.emit(false);
  }

}
