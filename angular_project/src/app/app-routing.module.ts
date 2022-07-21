import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

//added
import { RouterModule, Routes } from '@angular/router';
import { FeeListComponent } from './components/fee-list/fee-list.component';
import { FeeDetailsComponent } from './components/fee-details/fee-details.component';
import { CalculateFeeComponent } from './components/calculate-fee/calculate-fee.component';
import { ClientComponent } from './components/client/client.component';
import { SegmentComponent } from './components/segment/segment.component';

const routes: Routes = [
  { path: '', redirectTo: 'fees-charged', pathMatch: 'full' },
  { path: 'fees-charged', component: FeeListComponent },
  { path: 'fees-charged/:cpf/:id', component: FeeDetailsComponent },
  { path: 'calculate-fee', component: CalculateFeeComponent },
  { path: 'new-client', component: ClientComponent },
  { path: 'new-segment', component: SegmentComponent }
];


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
