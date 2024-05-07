import { Component } from '@angular/core';
import { CartServiceService } from '../../service/cartService/cart-service.service';
import { Tour } from '../../model/Tour';
import { NgFor } from '@angular/common';
import { TourService } from '../../service/tourService/tour.service';
import { TourPurchase } from '../../model/TourPurchase';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    NgFor
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {

  tours: Tour[] = [];
  selectedTour: Tour = new Tour(0,'','',0,0,[],false,false,false,'');

  constructor(private router: Router, public cartService: CartServiceService, private tourService: TourService){}

  ngOnInit(){
    this.tours = this.cartService.getCart();
  }

  selectTour(tour: Tour){
    this.selectedTour = tour;
  }

  removeFromCart(tour:Tour){
    this.cartService.removeFromCart(tour);
    this.ngOnInit()
  }

  purchaseTours(){
    this.cartService.sendMail(this.tours).then(() =>{
      console.log('mail sent');
    })
    .catch((error) => {
      console.error('error sending mail:', error);
    })
    this.tours.forEach((tour: Tour) => {
      this.tourService.purchaseTour(tour).subscribe(
        (data) => {
          console.log('tour purchased', data);
          this.cartService.emptyCart();
          this.tours = [];
          this.router.navigate(['/homepage'])
        },
        (error) => {
          console.error('error purchasing tour', error)
        }
      );
    });
    
  }
}
