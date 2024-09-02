import math

l = float(input('> Length of the polygons sides > ')) # Längen på en en månghörningens sida
n = int(input('> Number of vertices of the polygon > ')) # Nummer av hörn för mångmörningen
e = str(input('> Type of length type/greatness > ')) # Storhet

a = float((l*math.sin((n*180)/2*n)) * (l/math.sin(n*180/(2*n))) * 2 + (((l/math.sin(n*180/(2*n)))*2) * l)) # Månghörningens totala area

print('> Area of a ' + str(n) + ' sided polygon with ' + str(l) + ' ' + str(e) + ' sides: '  + str(a) + ' ' + str(e) + '2')