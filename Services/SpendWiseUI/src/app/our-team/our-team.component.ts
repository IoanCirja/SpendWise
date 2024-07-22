import { Component } from '@angular/core';

@Component({
  selector: 'app-our-team',
  templateUrl: './our-team.component.html',
  styleUrls: ['./our-team.component.scss']
})
export class OurTeamComponent {
  members = [
    { name: 'Barila Sabina Nadejda', imageUrl: "assets/Sabina.jpeg" ,description: 'Descriere pentru Barila Sabina Nadejda.'},
    { name: 'Cîrjă Ioan', imageUrl: "assets/Ioan.jpeg" ,description: 'Descriere pentru Barila Sabina Nadejda.'},
    { name: 'Cazamir Andrei', imageUrl: "assets/Sabina.jpeg" ,description: 'Descriere pentru Barila Sabina Nadejda.'},
    { name: 'Ghiuță Cristian-Daniel', imageUrl: "assets/Sabina.jpeg",description: 'Descriere pentru Barila Sabina Nadejda.' },
    { name: 'Huminiuc Simona', imageUrl: "assets/Simona.jpeg" ,description: 'Descriere pentru Barila Sabina Nadejda.'}
  ];
}
