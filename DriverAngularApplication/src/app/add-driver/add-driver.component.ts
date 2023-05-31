import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { GlobalService } from '../service/global.service';
import { driverForm } from '../models/Driver';

@Component({
  selector: 'app-add-driver',
  templateUrl: './add-driver.component.html',
  styleUrls: ['./add-driver.component.scss'],
})
export class AddDriverComponent implements OnInit {
  addDriverForm: driverForm = new driverForm();

  @ViewChild('driverForm')
  driverForm!: NgForm;

  isSubmitted: boolean = false;

  constructor(
    private router: Router,
    private globalService: GlobalService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  AddDriver(isValid: any) {
    this.isSubmitted = true;
    if (isValid) {
      this.globalService.post('Drivers/', this.addDriverForm).subscribe(
        async (data) => {
          if (data && data.isSuccess) {
            this.toastr.success(data.message);
            setTimeout(() => {
              this.router.navigate(['/Home']);
            }, 500);
          }
        },
        async (error) => {
          this.toastr.error(error.message);
          setTimeout(() => {
            this.router.navigate(['/Home']);
          }, 500);
        }
      );
    }
  }
}
