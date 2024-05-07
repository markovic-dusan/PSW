export class TourPurchase {
    constructor(
      public tourId: number,
      public userId: string | null,
      public dateOfPurchase: string
    ) {}
  }
  