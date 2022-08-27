import numpy as np


def generate_dict(mat_dimension):
    c = 'A'
    dictionary = {}
    for i in range(mat_dimension):
        x = chr(ord(c) + i)
        dictionary[i] = x
    return dictionary

def input_matrix(mat_dimension):
    dist_matrix = []
    for i in range(mat_dimension):
        inp = []
        for j in range(mat_dimension):
            x = int(input("Enter distance between " + dict[i] + " " + dict[j] + ": "))
            inp.append(x)
        dist_matrix.append(inp)
    return dist_matrix

dict = {}
result = []
distance_matrix = []

matrix_dimension = int(input("Enter matrix dimension: "))
dict = generate_dict(matrix_dimension)
distance_matrix = input_matrix(matrix_dimension)
print(dict)
while(len(distance_matrix) > 2):
    mini = 200000
    column1 = 0
    column2 = 0

    for i in range(len(distance_matrix)):                       #Find minimum value
        for j in range(i):
            if distance_matrix[i][j] < mini and distance_matrix[i][j] > 0:
                mini = distance_matrix[i][j]
                column2 = i
                column1 = j
    print(mini, dict[column1], dict[column2])

    cluster = []
    cluster.append(dict[column1])
    cluster.append(dict[column2])
    result.append(cluster)

    for i in range(len(distance_matrix)):
        for j in range(len(distance_matrix[i])):
            if i == column1:
                if j == column1:
                    distance_matrix[i][j] = 0
                else:
                    distance_matrix[i][j] = (distance_matrix[i][j] + distance_matrix[column2][j])/2
            else:
                if j == column1:
                    print(distance_matrix[i][j], distance_matrix[i][column2])
                    distance_matrix[i][j] = (distance_matrix[i][j]+distance_matrix[i][column2]) / 2
                if j == column2:
                    del(distance_matrix[i][j])

    del(distance_matrix[column1][column2])
    del(distance_matrix[column2])
    dict[column1] = dict[column1]+dict[column2]
    del(dict[column2])
    for i in range(column2+1, len(dict)+1):
        dict[i-1] = dict.pop(i) # 1 2 3 4
        # 0: AB, 1: C

    print(distance_matrix)
    print(dict)
    print(result)

final_cluster = []
final_cluster.append(dict[0])
final_cluster.append(dict[1])
result.append(final_cluster)
print(result)