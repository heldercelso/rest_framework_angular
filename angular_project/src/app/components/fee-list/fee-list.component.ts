import { Component, OnInit } from '@angular/core';

//added
import { FeesCharged } from 'src/app/models/feescharged.model';
import { FeesChargedService } from 'src/app/services/fees-charged.service';

@Component({
  selector: 'app-fee-list',
  templateUrl: './fee-list.component.html',
  styleUrls: ['./fee-list.component.css']
})
export class FeeListComponent implements OnInit {

  fees_charged?: FeesCharged[];
  currentFeeCharged: FeesCharged = {};
  currentIndex = -1;
  client = '';
  segment = '';
  constructor(private feeChargedService: FeesChargedService) { }

  ngOnInit(): void {
    this.retrieveFeeChargeds();
  }
  retrieveFeeChargeds(): void {
    this.feeChargedService.getAll()
      .subscribe({
        next: (data) => {
          this.fees_charged = data;
          console.log(data);
        },
        error: (e) => console.error(e)
      });
  }
  refreshList(): void {
    this.retrieveFeeChargeds();
    this.currentFeeCharged = {};
    this.currentIndex = -1;
  }
  setActiveFeeCharged(feeCharged: FeesCharged, index: number): void {
    this.currentFeeCharged = feeCharged;
    this.currentIndex = index;
  }
  removeAllFeeChargeds(): void {
    this.feeChargedService.deleteAll()
      .subscribe({
        next: (res) => {
          console.log(res);
          this.refreshList();
        },
        error: (e) => console.error(e)
      });
  }
  searchTitle(): void {
    this.currentFeeCharged = {};
    this.currentIndex = -1;
    this.feeChargedService.findByClientAndSegment(this.client, this.segment)
      .subscribe({
        next: (data) => {
          this.fees_charged = data;
          console.log(data);
        },
        error: (e) => console.error(e)
      });
  }
}
