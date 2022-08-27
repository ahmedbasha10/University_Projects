import numpy as np
import matplotlib.pyplot as plt
import biotite.sequence as seq
import biotite.sequence.align as align
import biotite.sequence.phylo as phylo
import biotite.sequence.graphics as graphics

# Obtain BLOSUM62          ///////////////////// BLAST project/////////////////////
matrix = align.SubstitutionMatrix.std_protein_matrix()

def remove_repeat(query):
    cnt = 0
    right = 0
    Query = list(query)
    for i in range(len(Query)-1):
        for j in range(i+1, len(Query)):
            if Query[i] == Query[j]:
                cnt += 1
                right = j
            else:
                break
        if cnt > 2:
            for k in range(i, right+1):
                Query[k] = 'X'
    query = "".join(Query)
    return query

def gettingScore(database_seq, query_seq):     # getting score from blosum matrix
    database_Sequence = list(database_seq)
    query_Sequence = list(query_seq)
    score = 0
    for i in range(len(database_seq)):
        score += matrix.get_score(database_Sequence[i], query_Sequence[i])
    return score

def getQindex(query, val):
    for i in range(len(query)-2):
        if val == query[i:i+3]:
            return i

def extend_seeds(seq, query, seed, q_word, left_idx, right_idx, q_left_idx, q_right_idx ,HSP):
    max_score = gettingScore(seed, q_word)  # get score between query word and seed
    max_left = left_idx + 1                # max left and right index for data base sequence
    max_right = right_idx - 1

    while (left_idx >= 0 and q_left_idx >= 0) or (right_idx < len(seq) and q_right_idx < len(query)):
        if left_idx >= 0 and q_left_idx >= 0:
            seed = seq[left_idx] + seed
            q_word = query[q_left_idx] + q_word
            left_idx -= 1
            q_left_idx -= 1
        if right_idx < len(seq) and q_right_idx < len(query):
            seed = seed + seq[right_idx]
            q_word = q_word + query[q_right_idx]
            right_idx += 1
            q_right_idx += 1

        new_score = gettingScore(seed, q_word)
        if new_score < max_score:
            if max_score - new_score > HSP:
                break
        else:
            max_score = new_score
            max_right = right_idx
            max_left = left_idx
    hsp_result = {"maxScore": max_score, "maxRight": max_right, "maxLeft": max_left}
    return hsp_result


def word_score(word, neighbor_word, T):
    List = []
    Word = list(word)
    Neigbor = list(neighbor_word)
    score = 0
    for i in range(0, 3):
        score += matrix.get_score(Word[i], Neigbor[i])
    if score >= T:
        List.append(neighbor_word)
        List.append(word)
    return List

query_sequencce =  input("enter the query sequence").upper()
threshold =  int(input("enter the threshold"))
#word_length=input("enter the word length")
hsp = int(input("enter hsp "))

query_sequencce = remove_repeat(query_sequencce)
words_list = []
amino_acid = ['A', 'R', 'N', 'D', 'C', 'Q', 'E', 'G', 'H', 'I', 'L', 'K', 'F', 'M', 'P', 'W', 'S', 'Y', 'T', 'V']
for i in range(len(query_sequencce)-2):
    word=""
    word+=query_sequencce[i:i+3]
    words_list.append(word)

ListOfNeighbors = []
ScoredList = {}
for word in words_list:                                  #pqgefg
    for i in range(0, 3):
        for j in amino_acid:
            neighbor = word
            neighborList = list(neighbor)
            neighborList[i] = j
            neighbor = "".join(neighborList)
            ListOfNeighbors.append(neighbor)

    for k in ListOfNeighbors:
        scored_word = []
        scored_word = word_score(word, k, threshold)
        if len(scored_word) != 0:
            ScoredList[scored_word[0]] = scored_word[1]
    ListOfNeighbors = []


file = open("Sequences.txt", "r")
database_sequence = []
database_sequence = file.readline().split(' ')

extend_result = {}
hsps_List = []
id = 1
for i in database_sequence:
    for key, value in ScoredList.items():
        for k in range(len(i) - 2):
            if key == i[k:k+3]:
                q_left_idx = getQindex(query_sequencce, value)
                extend_result = extend_seeds(i, query_sequencce, key, value, k-1, k+3, q_left_idx-1, q_left_idx+3, hsp)
                hspList = []
                if extend_result["maxScore"] >= threshold:
                    hspList.append(id)
                    hspList.append(extend_result["maxScore"])
                    hspList.append(i[extend_result["maxLeft"]:extend_result["maxRight"]+1])
                    id += 1
                    hsps_List.append(hspList)

if len(hsps_List) == 0:
    print("There is no query like this in data base")
else:
    print(hsps_List)