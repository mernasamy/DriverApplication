import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { GlobalService } from '../service/global.service';
import { driverForm } from '../models/Driver';

@Component({
  selector: 'app-edit-driver',
  templateUrl: './edit-driver.component.html',
  styleUrls: ['./edit-driver.component.scss'],
})
export class EditDriverComponent implements OnInit {
  editDriverForm: driverForm = new driverForm();

  @ViewChild('driverForm')
  driverForm!: NgForm;

  isSubmitted: boolean = false;
  id: any;

  constructor(
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private globalService: GlobalService
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.getDriverDetailById();
  }

  getDriverDetailById() {
    this.globalService.get('Drivers/' + this.id).subscribe(
      (data: any) => {
        if (data) {
          this.editDriverForm.Id = data.id;
          this.editDriverForm.FirstName = data.firstName;
          this.editDriverForm.LastName = data.lastName;
          this.editDriverForm.Email = data.email;
          this.editDriverForm.PhoneNumber = data.phoneNumber;
        }
      },
      (error: any) => {}
    );
  }

  EditDriver(isValid: any) {
    this.isSubmitted = true;
    if (isValid) {
      this.globalService
        .put('Drivers/' + this.id, this.editDriverForm)
        .subscribe(
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
