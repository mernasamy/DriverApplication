import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddDriverComponent } from './add-driver/add-driver.component';
import { EditDriverComponent } from './edit-driver/edit-driver.component';
import { HomeComponent } from './home/home.component';
import { ViewDriverComponent } from './view-driver/view-driver.component';

const routes: Routes = [
  { path: '', redirectTo: 'Home', pathMatch: 'full' },
  { path: 'Home', component: HomeComponent },
  { path: 'ViewDriver/:id', component: ViewDriverComponent },
  { path: 'AddDriver', component: AddDriverComponent },
  { path: 'EditDriver/:id', component: EditDriverComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
