export class Tour {
    constructor(
      public tourId: number,
      public name: string,
      public description: string,
      public difficulty: number,
      public price: number, 
      public interests: { interestValue: number }[], 
      public isDraft: Boolean,
      public isArchieved: Boolean,
      public isPublished: Boolean,
      public authorId: string | null,
    ) {}
  }
  