
export class ProblemStatusChange{
    constructor(
        public problemId: number,
        public newStatus: number,
        public timestamp: string,
    ){}
}