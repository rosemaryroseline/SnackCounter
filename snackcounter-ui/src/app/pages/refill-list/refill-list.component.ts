import { Component, OnInit } from '@angular/core';
import { RefillService } from '../../services/refill.service';
import { Refill } from '../../models/refill.model';

@Component({
  selector: 'app-refill-list',
  templateUrl: './refill-list.component.html',
  styleUrls: ['./refill-list.component.css']
})
export class RefillListComponent implements OnInit {

  refills: Refill[] = [];

  showForm: boolean = false;

  newRefill = {
    office: 1,
    createdByEmployeeId: ''
  };

  constructor(private refillService: RefillService) {}

  ngOnInit(): void {
    this.loadRefills();
  }

  loadRefills(): void {
    this.refillService.getRefills().subscribe({
      next: (data) => {
        this.refills = data;
      },
      error: (error) => {
        console.error('Error loading refills:', error);
      }
    });
  }

  createRefill(): void {

    if (!this.newRefill.createdByEmployeeId.trim()) {
      alert('Please enter Employee ID');
      return;
    }

    this.refillService.createRefill(this.newRefill).subscribe({
      next: (response) => {
        alert('Refill request created successfully!');

        this.showForm = false;

        this.newRefill = {
          office: 1,
          createdByEmployeeId: ''
        };

        this.loadRefills();
      },
      error: (error) => {
        alert(error.error?.message || 'Failed to create refill request');
        console.error(error);
      }
    });
  }

  getStatusName(status: number): string {
    switch (status) {
      case 0:
        return 'Open';
      case 1:
        return 'Assigned';
      case 2:
        return 'Accepted';
      case 3:
        return 'Declined';
      case 4:
        return 'Reimbursement Pending';
      case 5:
        return 'Completed';
      default:
        return 'Unknown';
    }
  }
}