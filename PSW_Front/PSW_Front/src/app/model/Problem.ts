
export class Problem{
    constructor(
        public problemId: number,
        public touristId: string | null,
        public title: string,
        public description: string,
        public tourId: number
    ){}
}