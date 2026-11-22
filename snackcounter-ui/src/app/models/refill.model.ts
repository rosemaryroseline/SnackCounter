export interface Refill {
  id: number;
  office: number;
  status: number;
  createdAt: string;
  acceptedAt: string | null;
  assignmentExpiresAt: string | null;
  completedAt: string | null;
  assignedEmployeeId: number | null;
  assignedEmployee: {
    id: number;
    name: string;
    employeeId: string;
    email: string;
    office: number;
    isActive: boolean;
    isAvailable: boolean;
  } | null;
  createdByEmployeeId: string;
  purchase: any | null;
}