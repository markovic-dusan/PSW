export class Keypoint {
    constructor(
      public pointId: number,
      public name: string,
      public description: string,
      public imageUrl: string,
      public latitude: number, 
      public longitude: number,
      public tourId: number
    ) {}
  }
  