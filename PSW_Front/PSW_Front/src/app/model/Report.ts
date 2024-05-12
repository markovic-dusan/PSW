import { Tour } from "./Tour";

export class Report{
    constructor(
        public reportId: number,
        public authorId: string,
        public date: string,
        public noOfSoldTours: number,
        public profit: number,
        public deltaProfit: number,
        public bestSellers: Tour[],
        public notSoldOnce: Tour[]
    ){}
}