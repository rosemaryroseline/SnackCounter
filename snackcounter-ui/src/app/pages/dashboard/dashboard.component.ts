import { Component, OnInit } from '@angular/core';
import { RefillService } from '../../services/refill.service';
import { Refill } from '../../models/refill.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  refills: Refill[] = [];
  loading: boolean = true;
  errorMessage: string = '';

  constructor(private refillService: RefillService) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading = true;

    this.refillService.getRefills().subscribe({
      next: (data) => {
        this.refills = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Dashboard loading error:', error);
        this.errorMessage = 'Unable to load dashboard data.';
        this.loading = false;
      }
    });
  }

  getTotalRefills(): number {
    return this.refills.length;
  }

  getOpenRefills(): number {
    return this.refills.filter(r => r.status === 0).length;
  }

  getAssignedRefills(): number {
    return this.refills.filter(r => r.status === 1).length;
  }

  getAcceptedRefills(): number {
    return this.refills.filter(r => r.status === 2).length;
  }

  getCompletedRefills(): number {
    return this.refills.filter(r => r.status === 5).length;
  }

  getPendingReimbursements(): number {
    return this.refills.filter(r => r.status === 4).length;
  }

  getOfficeRefills(office: number): number {
    return this.refills.filter(r => r.office === office).length;
  }

  getOfficeName(office: number): string {
    return office === 0 ? 'Oslo' : 'Bengaluru';
  }

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

  getRecentRefills(): Refill[] {
    return [...this.refills]
      .sort((a, b) =>
        new Date(b.createdAt).getTime() -
        new Date(a.createdAt).getTime()
      )
      .slice(0, 5);
  }
}