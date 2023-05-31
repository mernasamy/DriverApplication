import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalService } from '../service/global.service';

@Component({
  selector: 'app-view-driver',
  templateUrl: './view-driver.component.html',
  styleUrls: ['./view-driver.component.scss'],
})
export class ViewDriverComponent implements OnInit {
  id: any;
  driverDetail: any = [];

  constructor(
    private globalService: GlobalService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.getDriverDetailById();
  }

  getDriverDetailById() {
    this.globalService.get('Drivers/' + this.id).subscribe(
      (data: any) => {
        if (data) {
          this.driverDetail = data;
        }
      },
      (error: any) => {}
    );
  }
}
