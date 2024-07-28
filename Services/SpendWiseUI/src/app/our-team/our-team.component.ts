import { Component } from '@angular/core';

@Component({
  selector: 'app-our-team',
  templateUrl: './our-team.component.html',
  styleUrls: ['./our-team.component.scss']
})
export class OurTeamComponent {
  members = [
    { name: 'Barila Sabina Nadejda', imageUrl: "assets/Sabina.jpeg" ,description: 'Enthusiastic student passionate about fashion, reading and outdoor walks. I play piano and share positive energy. Apparently, I like the frontend more than the backend.'},
    { name: 'Cîrjă Ioan', imageUrl: "assets/Ioan.jpeg" ,description: 'Empathic thinker with a keen interest in the latest technologies, personal development, and a commitment to excellence. P.S. I also enjoy reading and campfires.'},
    { name: 'Cazamir Andrei', imageUrl: "assets/Andrei.jpeg" ,description: 'I am passionate about traveling, football and video games, spending my free time exploring new places, watching football matches and gaming.'},
    { name: 'Ghiuță Cristian-Daniel', imageUrl: "assets/Cristi.jpeg",description: 'Descriere pentru Barila Sabina Nadejda.' },
    { name: 'Huminiuc Simona', imageUrl: "assets/Simona.jpeg" ,description: 'Descriere pentru Barila Sabina Nadejda.'}
  ];
}
