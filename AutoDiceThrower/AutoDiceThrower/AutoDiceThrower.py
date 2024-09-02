
import random
import time

while True:
    time.sleep(0.1)
    num_tries = int(input('> num_tries = '))
    

    num_dice = 0
    num_amounts = [0] * 7  # Initialize counts for dice rolls

    for i in range(num_tries):
        num_dice = random.randint(1, 6)
        num_amounts[num_dice] += 1  # Increment count for the dice roll

    print('> ---------------------------------')
    print('> ' + str(num_amounts[1:]))  # Print counts for each dice roll
    for b in range(1, 7):
        percentage = (num_amounts[b] / num_tries) * 100
        print('> ' + str(b) + ': ' + str(percentage) + '% of tries')