using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
    void Init()
    {
        Evaluate();
    }

    void Evaluate()
    {
        for (int i = 0; i < options.popSize; i++)
        {
            members[i]->fitness = Eval(members[i]);
        }
    }

    void Statistics()
    {
        sumFitness = 0;
        min = members[0]->fitness;
        max = members[0]->fitness;
        for (int i = 0; i < options.popSize; i++)
        {
            sumFitness += members[i]->fitness;
            if (min > members[i]->fitness)
                min = members[i]->fitness;
            if (max < members[i]->fitness)
                max = members[i]->fitness;
        }
        avg = sumFitness / options.popSize;
    }

    void Report(unsigned long int gen)
    {
        char printbuf[1024];
        sprintf(printbuf, "%4i \t %f \t %f \t %f\n ", (int)gen, min, avg, max);
        WriteBufToFile(std::string(printbuf), options.outfile);
        std::cout << printbuf;
    }

    void Generation(Population* child)
    {
        int pi1, pi2, ci1, ci2;
        Individual* p1, *p2, *c1, *c2;
        for (int i = 0; i < options.popSize; i += 2)
        {
            pi1 = ProportionalSelector();
            pi2 = ProportionalSelector();

            ci1 = i;
            ci2 = i + 1;

            p1 = members[pi1]; p2 = members[pi2];
            c1 = child->members[ci1]; c2 = child->members[ci2];

            XoverAndMutate(p1, p2, c1, c2);
        }
    }



    int ProportionalSelector()
    {
        int i = -1;
        float sum = 0;
        float limit = RandomFraction() * sumFitness;
        do
        {
            i = i + 1;
            sum += members[i]->fitness;
        } while (sum < limit && i < options.popSize - 1);

        return i;
    }

    void XoverAndMutate(Individual* p1, Individual* p2, Individual* c1, Individual* c2)
    {

        for (int i = 0; i < options.chromLength; i++)
        { //First copy
            c1->chromosome[i] = p1->chromosome[i];
            c2->chromosome[i] = p2->chromosome[i];
        }
        if (Flip(options.px))
        { // if prob, then cross/exchange bits
            OnePoint(p1, p2, c1, c2);
        }

        c1->Mutate(options.pm);
        c2->Mutate(options.pm);
    }

    void OnePoint(Individual* p1, Individual* p2, Individual* c1, Individual* c2)
    { //not debugged
        int t1 = IntInRange(0, options.chromLength);
        for (int i = t1; i < options.chromLength; i++)
        {
            c1->chromosome[i] = p2->chromosome[i];
            c2->chromosome[i] = p1->chromosome[i];
        }
    }

    void TwoPoint(Individual* p1, Individual* p2, Individual* c1, Individual* c2)
    { //not debugged
        int t1 = IntInRange(0, options.chromLength);
        int t2 = IntInRange(0, options.chromLength);
        int xp1 = std::fmin(t1, t2);
        int xp2 = std::fmax(t1, t2);
        for (int i = xp1; i < xp2; i++)
        {
            c1->chromosome[i] = p2->chromosome[i];
            c2->chromosome[i] = p1->chromosome[i];
        }
    }
}
