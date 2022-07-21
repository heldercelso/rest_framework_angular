import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
//added
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { CalculateFeeComponent } from './components/calculate-fee/calculate-fee.component';
import { FeeDetailsComponent } from './components/fee-details/fee-details.component';
import { FeeListComponent } from './components/fee-list/fee-list.component';
import { AppRoutingModule } from './app-routing.module';
import { ClientComponent } from './components/client/client.component';
import { SegmentComponent } from './components/segment/segment.component';

@NgModule({
  declarations: [
    AppComponent,
    CalculateFeeComponent,
    FeeDetailsComponent,
    FeeListComponent,
    ClientComponent,
    SegmentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    //added
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
