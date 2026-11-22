import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface MonthlyReport {
  office: string;
  currency: string;
  monthlyBudget: number;
  totalSpent: number;
  remainingBudget: number;
  totalRefills: number;
  pendingReimbursements: number;
}

@Component({
  selector: 'app-monthly-reports',
  templateUrl: './monthly-reports.component.html',
  styleUrls: ['./monthly-reports.component.css']
})
export class MonthlyReportsComponent implements OnInit {

  reports: MonthlyReport[] = [];
  loading: boolean = false;
  errorMessage: string = '';

  selectedMonth: number = new Date().getMonth() + 1;
  selectedYear: number = new Date().getFullYear();

  months = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' }
  ];

  years: number[] = [];

  private apiUrl = 'https://localhost:7190/api/Reports/monthly';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    const currentYear = new Date().getFullYear();

    for (let year = currentYear - 2; year <= currentYear + 1; year++) {
      this.years.push(year);
    }

    this.loadReports();
  }

  loadReports(): void {
    this.loading = true;
    this.errorMessage = '';

    this.http.get<MonthlyReport[]>(this.apiUrl, {
      params: {
        year: this.selectedYear,
        month: this.selectedMonth
      }
    }).subscribe({
      next: (data) => {
        this.reports = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading monthly reports:', error);
        this.errorMessage = 'Failed to load monthly reports.';
        this.loading = false;
      }
    });
  }

  getTotalBudget(): number {
    return this.reports.reduce(
      (total, report) => total + report.monthlyBudget,
      0
    );
  }

  getTotalSpent(): number {
    return this.reports.reduce(
      (total, report) => total + report.totalSpent,
      0
    );
  }

  getTotalRemaining(): number {
    return this.reports.reduce(
      (total, report) => total + report.remainingBudget,
      0
    );
  }

  getTotalRefills(): number {
    return this.reports.reduce(
      (total, report) => total + report.totalRefills,
      0
    );
  }

  getTotalPendingReimbursements(): number {
    return this.reports.reduce(
      (total, report) => total + report.pendingReimbursements,
      0
    );
  }

  getSpentPercentage(report: MonthlyReport): number {
    if (report.monthlyBudget === 0) {
      return 0;
    }

    return Math.min(
      (report.totalSpent / report.monthlyBudget) * 100,
      100
    );
  }

  getMonthName(): string {
    return this.months.find(
      month => month.value === this.selectedMonth
    )?.name || '';
  }

  formatCurrency(amount: number, currency: string): string {
    return new Intl.NumberFormat('en', {
      style: 'currency',
      currency: currency
    }).format(amount);
  }

}