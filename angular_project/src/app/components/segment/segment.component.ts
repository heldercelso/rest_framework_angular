import { Component, OnInit } from '@angular/core';
import { Segment } from 'src/app/models/segment.model';
import { SegmentService } from 'src/app/services/segment.service';


@Component({
  selector: 'app-segment',
  templateUrl: './segment.component.html',
  styleUrls: ['./segment.component.css']
})
export class SegmentComponent implements OnInit {
  segment: Segment = {
    name: '',
    fee: '',
  };
  submitted = false;
  error_msg = "";

  constructor(private segmentService: SegmentService) { }

  ngOnInit(): void {
  }
  saveSegment(): void {
    const data = {
      name: this.segment.name,
      fee: this.segment.fee,
    };
    this.segmentService.create(data)
      .subscribe({
        next: (res) => {
          console.log(res);
          this.submitted = true;
        },
        error: (e) => {
          console.error(e);
          if (e.error.errors) {
            for (let err in e.error.errors) {
              if (!err.startsWith("$")) {
                this.error_msg = e.error.errors[err];
                return;
              }
              if (e.error.errors["$.fee"]) {
                this.error_msg = "Campo Taxa inválido.";
                return;
              }
            }
          }
          if (typeof e.error === 'string' || e.error instanceof String) {
            this.error_msg = "Segmento com este nome já foi cadastrado anteriormente.";
          } else {
            for (let err in e.error) {
              this.error_msg = e.error[err];
              return;
            }
          }

          if (this.error_msg == "")
            this.error_msg = "Erro desconhecido. Confira os campos e tente novamente.";
          
        }
      });
      this.error_msg = "";
  }
  newSegment(): void {
    this.submitted = false;
    this.segment = {
      name: '',
      fee: '',
    };
  }
}
