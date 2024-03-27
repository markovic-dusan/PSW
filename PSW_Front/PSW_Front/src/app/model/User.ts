export class User {
  constructor(
    public userName: string,
    public normalizedUserName: string,
    public password: string,
    public userType: number,
    public name: string, 
    public lastName: string,
    public interests: { interestValue: number }[], 
    public email: string
  ) {}
}
