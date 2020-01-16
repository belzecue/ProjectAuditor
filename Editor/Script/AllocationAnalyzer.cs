using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Unity.ProjectAuditor.Editor
{
    [ScriptAnalyzer]
    public class AllocationAnalyzer : IInstructionAnalyzer
    {
        private static readonly ProblemDescriptor objectAllocationDescriptor = new ProblemDescriptor
        {
            id = 102002,
            description = "Object Allocation",
            type = string.Empty,
            method = string.Empty,
            area = "Memory",
            problem = "TODO",
            solution = "TODO"
        };

        private static readonly ProblemDescriptor arrayAllocationDescriptor = new ProblemDescriptor
        {
            id = 102003,
            description = "Array Allocation",
            type = string.Empty,
            method = string.Empty,
            area = "Memory",
            problem = "TODO",
            solution = "TODO"
        };

        public AllocationAnalyzer(ScriptAuditor auditor)
        {
            auditor.RegisterDescriptor(objectAllocationDescriptor);
            auditor.RegisterDescriptor(arrayAllocationDescriptor);
        }
        
        public ProjectIssue Analyze(MethodDefinition methodDefinition, Instruction inst)
        {
            if (inst.OpCode == OpCodes.Newobj)
            {
                var methodReference = (MethodReference)inst.Operand;
                var typeReference = methodReference.DeclaringType;
                if (typeReference.IsValueType)
                    return null;

                var descriptor = objectAllocationDescriptor;
                var description = string.Format("'{0}' object allocation", typeReference.Name);
                
                var calleeNode = new CallTreeNode(descriptor.description);
            
                return new ProjectIssue
                (
                    descriptor,
                    description,
                    IssueCategory.Allocations,
                    calleeNode
                );                
            }
            else // OpCodes.Newarr
            {
                var typeReference = (TypeReference)inst.Operand;
                var descriptor = arrayAllocationDescriptor;
                var description = string.Format("'{0}' array allocation", typeReference.Name);
                
                var calleeNode = new CallTreeNode(descriptor.description);
        
                return new ProjectIssue
                (
                    descriptor,
                    description,
                    IssueCategory.Allocations,
                    calleeNode
                );                
            }
        }

        public IEnumerable<OpCode> GetOpCodes()
        {
            yield return OpCodes.Newobj;
            yield return OpCodes.Newarr;
        }
    }
}