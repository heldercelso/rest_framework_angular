import { Component, OnInit } from '@angular/core';

//added
import { FeesCharged } from 'src/app/models/feescharged.model';
import { FeesChargedService } from 'src/app/services/fees-charged.service';

@Component({
  selector: 'app-calculate-fee',
  templateUrl: './calculate-fee.component.html',
  styleUrls: ['./calculate-fee.component.css']
})
export class CalculateFeeComponent implements OnInit {

  fees_charged: FeesCharged = {
    client: '',
    source_currency_amount: '',
  };
  submitted = false;
  error_msg = ""
  constructor(private feesChargedService: FeesChargedService) { }

  ngOnInit(): void {
  }
  saveFeesCharged(): void {
    const data = {
      client: this.fees_charged.client,
      source_currency_amount: this.fees_charged.source_currency_amount
    };
    this.feesChargedService.create(data)
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
              if (e.error.errors["$.source_currency_amount"]) {
                this.error_msg = "Quantidade inválida.";
                return;
              }
              if (e.error.errors["$.client"]) {
                this.error_msg = "Cpf inválido.";
                return;
              }
            }
          }
          if (typeof e.error === 'string' || e.error instanceof String) {
            this.error_msg = "Cpf não cadastrado.";
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
  newFeesCharged(): void {
    this.submitted = false;
    this.fees_charged = {
      client: '',
      source_currency_amount: '',
    };
  }

}
