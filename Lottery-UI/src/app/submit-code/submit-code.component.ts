import { Component, OnInit } from '@angular/core';
import { IUserCode, ICode } from '../winners-list/winners-list.model';
import { SubmitCodeService } from './submit-code.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-submit-code',
  templateUrl: './submit-code.component.html',
  styleUrls: ['./submit-code.component.css']
})
export class SubmitCodeComponent implements OnInit {
  userCode: IUserCode = {} as IUserCode;

  constructor(private submitCodeService: SubmitCodeService,
    private toastrService: ToastrService,
    private router: Router) {
    this.userCode.code = {} as ICode;
  }

  ngOnInit() {
  }

  submit() {
    this.submitCodeService.submitCode(this.userCode).subscribe((result) => {
      if(!!result) {
        this.toastrService.success(result.awardDescription, "Congartulations!");
      } else {
        this.toastrService.info("Better luck next time", "Info");
      }
      this.router.navigate(['winners']);
    }, (error) => {
      this.toastrService.error(error.error.exceptionMessage, "Error!")
    })
  }
}
