
import { Component, OnInit } from '@angular/core';
import { RefillService } from '../../services/refill.service';
import { Refill } from '../../models/refill.model';

@Component({
  selector: 'app-my-assignments',
  templateUrl: './my-assignments.component.html',
  styleUrls: ['./my-assignments.component.css'
  ]
})
export class MyAssignmentsComponent implements OnInit {

  assignments: Refill[] = [];
  employeeId: number = 1;

  // Purchase form
  isPurchaseFormVisible = false;
  selectedRefillId: number | null = null;

  purchaseData = {
    employeeId: 1,
    amount: 0,
    description: '',
    receiptPath: ''
  };

  constructor(private refillService: RefillService) {}

  ngOnInit(): void {
    this.loadAssignments();
  }

  // Load assignments for current employee
  loadAssignments(): void {
    this.refillService.getRefills().subscribe({
      next: (data) => {
        this.assignments = data.filter(
          refill => refill.assignedEmployeeId === this.employeeId
        );
      },
      error: (error) => {
        console.error('Error loading assignments:', error);
      }
    });
  }

  // Status display
  getStatusName(status: number): string {
    switch (status) {
      case 0: return 'Open';
      case 1: return 'Assigned';
      case 2: return 'Accepted';
      case 3: return 'Declined';
      case 4: return 'Reimbursement Pending';
      case 5: return 'Completed';
      default: return 'Unknown';
    }
  }

  // Accept assignment
  acceptRefill(refillId: number): void {
    this.refillService.acceptRefill(refillId, this.employeeId).subscribe({
      next: () => {
        alert('Refill accepted successfully!');
        this.loadAssignments();
      },
      error: (error) => {
        console.error(error);
        alert(error.error?.message || 'Failed to accept refill.');
      }
    });
  }

  // Decline assignment
  declineAssignment(refillId: number): void {
    if (!confirm('Are you sure you want to decline this assignment?')) {
      return;
    }

    this.refillService.declineRefill(refillId, this.employeeId).subscribe({
      next: () => {
        alert('Assignment declined successfully!');
        this.loadAssignments();
      },
      error: (error) => {
        console.error(error);
        alert(error.error?.message || 'Failed to decline assignment.');
      }
    });
  }

  // Open purchase form
  showPurchaseForm(refillId: number): void {
    this.selectedRefillId = refillId;
    this.isPurchaseFormVisible = true;

    this.purchaseData = {
      employeeId: this.employeeId,
      amount: 0,
      description: '',
      receiptPath: ''
    };
  }

  // Close purchase form
  cancelPurchase(): void {
    this.isPurchaseFormVisible = false;
    this.selectedRefillId = null;
  }

  // Submit purchase
  submitPurchase(): void {
    if (this.selectedRefillId === null) {
      return;
    }

    if (this.purchaseData.amount <= 0) {
      alert('Enter a valid purchase amount.');
      return;
    }

    if (!this.purchaseData.description.trim()) {
      alert('Enter purchase description.');
      return;
    }

    this.refillService.purchaseRefill(
      this.selectedRefillId,
      this.purchaseData
    ).subscribe({
      next: () => {
        alert('Purchase submitted successfully!');
        this.isPurchaseFormVisible = false;
        this.selectedRefillId = null;
        this.loadAssignments();
      },
      error: (error) => {
        console.error(error);
        alert(error.error?.message || 'Failed to submit purchase.');
      }
    });
  }
}
