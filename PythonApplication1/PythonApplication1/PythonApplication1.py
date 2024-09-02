import array
import random

while True:
    num_tries = int(input('> num_tries = ')) ## number of dice throws
    
    num_check = 0
    num_dice = int()
    num_percentage = float()
    array_tries = []
    array_percentages = [0,1,2,3,4,5]
    
    for i in range (num_tries):
        num_dice = random.randint(1,6)
        array_tries.append(num_dice)
    for a in range (len(array_tries) + 1):
        for b in range (1, 6):
            if b == array_tries[a]:
                pass
                

    print('> ' + str(array_tries))
    print('> ' + str(len(array_tries)) + ': ' tries')
    print('> ---------------------------------')