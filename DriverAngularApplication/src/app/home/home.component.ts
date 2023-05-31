import { Component, Input, OnInit, Type } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { GlobalService } from '../service/global.service';

@Component({
  selector: 'ng-modal-confirm',
  template: `
    <div class="modal-header">
      <h5 class="modal-title" id="modal-title">Delete Confirmation</h5>
      <button
        type="button"
        class="btn close"
        aria-label="Close button"
        aria-describedby="modal-title"
        (click)="modal.dismiss('Cross click')"
      >
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
    <div class="modal-body">
      <p>Are you sure you want to delete?</p>
    </div>
    <div class="modal-footer">
      <button
        type="button"
        class="btn btn-outline-secondary"
        (click)="modal.dismiss('cancel click')"
      >
        CANCEL
      </button>
      <button
        type="button"
        ngbAutofocus
        class="btn btn-success"
        (click)="modal.close('Ok click')"
      >
        OK
      </button>
    </div>
  `,
})
export class NgModalConfirm {
  constructor(public modal: NgbActiveModal) {}
}

const MODALS: { [name: string]: Type<any> } = {
  deleteModal: NgModalConfirm,
};

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  closeResult = '';
  driverList: any = [];
  constructor(
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private globalService: GlobalService
  ) {}

  ngOnInit(): void {
    this.getAllDriver();
  }

  async getAllDriver() {
    this.globalService.get('Drivers/').subscribe(
      (data: any) => {
        if (data) {
          this.driverList = data;
        }
      },
      (error: any) => {
        if (error) {
          if (error.status == 404) {
            if (error.error && error.error.message) {
              this.driverList = [];
            }
          }
        }
      }
    );
  }

  AddDriver() {
    this.router.navigate(['AddDriver']);
  }
  AddDriverList() {
    this.globalService
      .post('Drivers/CreateRandomList?listLength=' + 100)
      .subscribe(
        async (data) => {
          if (data && data.isSuccess) {
            this.toastr.success(data.message);
            this.getAllDriver();
          }
        },
        async (error) => {
          this.toastr.error(error.message);
        }
      );
  }
  deleteDriverConfirmation(driver: any) {
    this.modalService
      .open(MODALS['deleteModal'], {
        ariaLabelledBy: 'modal-basic-title',
      })
      .result.then(
        (result) => {
          this.deleteDriver(driver);
        },
        (reason) => {}
      );
  }

  deleteDriver(driver: any) {
    this.globalService.delete('Drivers/' + driver.id).subscribe(
      (data: any) => {
        if (data && data.isSuccess == true) {
          this.toastr.success(data.message);
          this.getAllDriver();
        }
      },
      (error: any) => {}
    );
  }

  orderDriverName(driver: any) {
    this.driverList.find((drv: any) => drv.id === driver.id).firstName =
      driver.firstName
        .split('')
        .sort((a: string, b: string) =>
          a.localeCompare(b, 'en', { sensitivity: 'base' })
        )
        .join('');
    this.driverList.find((drv: any) => drv.id === driver.id).lastName =
      driver.lastName
        .split('')
        .sort((a: string, b: string) =>
          a.localeCompare(b, 'en', { sensitivity: 'base' })
        )
        .join('');
  }

  orderDrivers() {
    try {
      this.driverList.sort((a: any, b: any) =>
        a.firstName.localeCompare(b.firstName, 'en', { sensitivity: 'base' })
      );
    } catch (error) {
      console.log(error);
    }
  }
}
