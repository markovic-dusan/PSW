import { Injectable } from '@angular/core';
import { Tour } from '../../model/Tour';
import emailjs from '@emailjs/browser'

@Injectable({
  providedIn: 'root'
})
export class CartServiceService {  
  private tours: Tour[] = [];
  public  totalPrice: number = 0;
  constructor() { }

  addToCart(tour: Tour){
    this.tours.push(tour);
    this.totalPrice+=tour.price;
  }

  removeFromCart(tour: Tour) {
    const index = this.tours.indexOf(tour);
    if (index !== -1) {
      this.tours.splice(index, 1);
      this.totalPrice-=tour.price;
    }
  }

  getCart() {
    return this.tours;
  }

  emptyCart(){
    this.tours = [];
    this.totalPrice = 0;
  }

  async sendMail(tours: Tour[]){
    emailjs.init('I_N-Q7jZxOfozBdZo')
    console.log(tours.length)
    var to = localStorage.getItem('email') !== null ? localStorage.getItem('email') as string : '';
    var message: string = ''
    tours.forEach(tour => {
      if (tour.name) {
        message += "|" + tour.name + "|";
      }
    });
    console.log(message)
  
    emailjs.send("service_bm7fbu6","template_obxvgvd",{
      message: message,
      reply_to: "",
      to: to,
      });
  }
}
