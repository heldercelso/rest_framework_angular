import { Component, Input, OnInit } from '@angular/core';
import { FeesChargedService } from 'src/app/services/fees-charged.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FeesCharged } from 'src/app/models/feescharged.model';

@Component({
  selector: 'app-fee-details',
  templateUrl: './fee-details.component.html',
  styleUrls: ['./fee-details.component.css']
})
export class FeeDetailsComponent implements OnInit {

  @Input() viewMode = false;

  @Input() currentFeeCharged: FeesCharged = {
    client: '',
    client_name: '',
    client_segment: '',
    segment_fee: '',
    source_currency_amount: '',
    conversion_result: '',
    fee: '',
    formula: '',
  };
  
  message = '';
  error_msg = "";

  constructor(
    private feeDetailService: FeesChargedService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    if (!this.viewMode) {
      this.message = '';
      this.getFeeCharged(this.route.snapshot.params["cpf"], this.route.snapshot.params["id"]);
    }
  }

  getFeeCharged(cpf: string, id: string): void {
    this.feeDetailService.get(cpf, id)
      .subscribe({
        next: (data) => {
          this.currentFeeCharged = data;
          console.log(data);
        },
        error: (e) => console.error(e)
      });
  }

  updateFeeCharged(): void {
    this.message = '';
    this.feeDetailService.update(this.currentFeeCharged.client, this.currentFeeCharged.id, this.currentFeeCharged)
      .subscribe({
        next: (res) => {
          console.log(res);
          //this.message = res.message ? res.message : 'Atualizado com sucesso!';
          this.message = 'Atualizado com sucesso!';
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
            }
          }
          if (typeof e.error === 'string' || e.error instanceof String) {
            this.error_msg = "Cpf Inválido.";
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

  deleteFeeCharged(): void {
    this.feeDetailService.delete(this.currentFeeCharged.client, this.currentFeeCharged.id)
      .subscribe({
        next: (res) => {
          console.log(res);
          this.router.navigate(['/fees-charged']);
        },
        error: (e) => console.error(e)
      });
  }

}
