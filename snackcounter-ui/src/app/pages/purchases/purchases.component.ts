import { Component, OnInit } from '@angular/core';
import { RefillService } from '../../services/refill.service';
import { Refill } from '../../models/refill.model';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html',
  styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent implements OnInit {

  purchases: Refill[] = [];
  loading: boolean = true;

  constructor(private refillService: RefillService) {}

  ngOnInit(): void {
    this.loadPurchases();
  }

  loadPurchases(): void {
    this.loading = true;

    this.refillService.getRefills().subscribe({
      next: (data) => {

        // Show refills that have reached the purchase/reimbursement stage
        this.purchases = data.filter(
          refill => refill.status === 4 || refill.status === 5
        );

        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading purchases:', error);
        this.loading = false;
      }
    });
  }

  getStatusName(status: number): string {
    switch (status) {
      case 4:
        return 'Reimbursement Pending';

      case 5:
        return 'Completed';

      default:
        return 'Unknown';
    }
  }

  getOfficeName(office: number): string {
    switch (office) {
      case 0:
        return 'Oslo';

      case 1:
        return 'Bengaluru';

      default:
        return 'Unknown';
    }
  }

}