// Modules.
var util = require('util');

// Constants.
var TRACK_MAX_TOTAL_LENGTH = 100000.00, // m
    TRACK_SECTION_MIN_LENGTH = 500.00, // m
    TRACK_SECTION_MAX_QTY = 5,
    TRACK_SECTION_SLOW_LENGTH, // Slow is defined as not max speed.
    TRACK_SECTION_SLOW_DURATION,
    STATION_LENGTH = 150.00, // m
    STATION_WAIT_TIME = 1, // s
    TRAIN_MAX_QTY = 5,  
    TRAIN_LENGTH = 100.00, // m
    TRAIN_ACCELERATION = 0.75, // m/s²
    TRAIN_DECELERATION = -(19 / 18), // m/s²
    TRAIN_MAX_SPEED = 25.00, // m/s
    TRAIN_STOP_TRESHOLD = 0.00, // s
    TRAIN_STOP_DURATION = 120.00; // s

// Compute minimum distance required.
//TRACK_SECTION_SLOW_LENGTH = 712.73;
//TRACK_SECTION_SLOW_DURATION = 59.72;


TRACK_SECTION_SLOW_DURATION = TRAIN_MAX_SPEED / TRAIN_ACCELERATION;
TRACK_SECTION_SLOW_LENGTH = TRACK_SECTION_SLOW_DURATION * TRAIN_MAX_SPEED * 0.5;
var trailingLength = (-Math.pow(TRAIN_MAX_SPEED, 2) * 0.5)/TRAIN_DECELERATION;
TRACK_SECTION_SLOW_LENGTH += trailingLength;
TRACK_SECTION_SLOW_DURATION += (2 * trailingLength)/TRAIN_MAX_SPEED;

// Returns time in s spent by a train in a section of length
// sectionLength when accelerating/decelerating optimally.
// Does not account for boundary effects.
function trainSectionDuration(sectionLength) {
  var time = 0, // s
      meetingTime, // s
      meetingSpeed; // m/s

  if (sectionLength >= TRACK_SECTION_SLOW_LENGTH) {
    // We know the length and duration of the variable part, the rest is an easy
    // calculation.
    sectionLength -= TRACK_SECTION_SLOW_LENGTH;
    time += TRACK_SECTION_SLOW_DURATION;

    // Cruise speed.
    time += sectionLength / TRAIN_MAX_SPEED;
  } else {  
    meetingDistance = sectionLength/((TRAIN_ACCELERATION/-TRAIN_DECELERATION) + 1);
    meetingSpeed = Math.sqrt(2 * TRAIN_ACCELERATION * meetingDistance);
    time += meetingSpeed/TRAIN_ACCELERATION;
    time += -meetingSpeed/TRAIN_DECELERATION;
  }

  return time;
}

function processData(input) {
    var problemData = input.split(' '),
        trainCount,
        trainsLeft,
        trackSections = [],
        trains = [],
        totalTrackLength = 0;

    if (input.length < 3) {
      console.log('ERROR');
      return;
    }

    if (problemData.length < 2) {
      console.log('ERROR');
      return;
    }

    trainsLeft = trainCount = parseInt(problemData.shift(), 10);

    if (trainCount < 1 || trainCount > TRAIN_MAX_QTY) {
      console.log('ERROR');
      return;
    }

    for (var i = 0; i < trainCount; i++) {
      trains.push({
        departures: [],
        arrivals: []
      });
    }

    while (problemData.length !== 0) {
      var newTrackSegment = problemData.shift();

      if ((Number(newTrackSegment) % 1) !== 0) {
        console.log('ERROR');
        return;
      }

      newTrackSegment = parseInt(newTrackSegment, 10);

      if (newTrackSegment < 500) {
        console.log('ERROR');
        return;
      }

      totalTrackLength += newTrackSegment;

      trackSections.push({
        trackLength: newTrackSegment,
        currentTrain: null,
        lastExit: STATION_WAIT_TIME
      });
    }

    if (totalTrackLength !== 100000) {
      console.log('ERROR');
      return;
    }

    if (trackSections.length < 1 || trackSections.length > TRACK_SECTION_MAX_QTY) {
      console.log('ERROR');
      return;
    }

    /*
    for (var i = 500; i <= 3000; i+=25 ) {
      console.log('%d seconds for %d meters', Math.round(trainSectionDuration(i) + 1), i);
    }
    */

    (function moveTrains() {
      trackSections.forEach( function (trackSection, index) {
        if (!trackSection.currentTrain) {
          if (index === 0) {
            // Depot.
            if( trainsLeft > 0 ) {
              trackSection.currentTrain = trainCount - trainsLeft;
              trainsLeft--;
              var newDeparture = 1;

              if (trackSection.currentTrain !== 0) {
                var previousTrain = trains[trackSection.currentTrain - 1];
                newDeparture += previousTrain.departures[1];
              }
              
              trains[trackSection.currentTrain].departures.push(newDeparture);
              trackSection.lastExit = newDeparture;

              var duration = Math.round(trainSectionDuration(trackSection.trackLength));
              var newArrival = newDeparture + duration;
              trains[trackSection.currentTrain].arrivals.push(newArrival);
            }
            else {
              console.log('No more new trains!');
            }
          } else if(index < (trackSections.length)) {
            // Station.
            var previousSection = trackSections[index - 1];
            trackSection.currentTrain = previousSection.currentTrain;

            var previousArrivals = trains[previousSection.currentTrain].arrivals;
            var duration = Math.round(trainSectionDuration(trackSection.trackLength));

            var newDeparture;
            if (trackSection.currentTrain === 0) {
              newDeparture = previousArrivals[previousArrivals.length - 1] + TRAIN_STOP_DURATION + STATION_WAIT_TIME;
            } else {
              var previousTrain = trains[trackSection.currentTrain - 1];
              if (index === (trackSections.length - 1)) {
                newDeparture = 1 + previousTrain.arrivals[index];
              } else {
                newDeparture = 1 + previousTrain.departures[index + 1];
              }

              newDeparture = Math.max(newDeparture, previousArrivals[previousArrivals.length - 1] + TRAIN_STOP_DURATION + STATION_WAIT_TIME);
            }

            trains[trackSection.currentTrain].departures.push(newDeparture);
            trains[trackSection.currentTrain].arrivals.push(newDeparture + duration);
            previousSection.currentTrain = null;
          } 
          
          // Could have only Depot and Terminal...
          if (index === (trackSections.length - 1)) {
            // Terminal.
            trackSection.currentTrain = null;
            if( trainsLeft !== 0 ) {
              //TODO.
              moveTrains();
            }
          }
          
        } else {
          console.log('We have a train!');
        }
      });
    })();

    //console.log(util.inspect(trains));

    //console.log('We have %d trains and %d sections that total %d kms', trainCount, trackSections.length, totalTrackLength / 1000);

    trains.forEach( function (train, index) {
      var output = '' + (index + 1) + ' :';
      
      for(var j = 0; j < trackSections.length; j++) {
        if (j === 0) {
          output += ' ***** - ' + ('     ' + train.departures.shift()).slice(-5) + ' ';
        } else {
          output += ' ' + ('     ' + train.arrivals.shift()).slice(-5) + ' - ' + ('     ' + train.departures.shift()).slice(-5) + ' ';
        }

        if (j === (trackSections.length - 1)) {
          output += ' ' + ('     ' + train.arrivals.shift()).slice(-5) + ' *****';
        }
      }

      console.log(output);
    });
} 

process.stdin.resume();
process.stdin.setEncoding("ascii");
_input = "";
process.stdin.on("data", function (input) {
    _input += input;
});

process.stdin.on("end", function () {
   processData(_input.replace(/(\r\n|\n|\r)/gm,''));
});
