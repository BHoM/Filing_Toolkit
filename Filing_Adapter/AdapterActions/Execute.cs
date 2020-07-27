using BH.Engine.Filing;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Filing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Base;
using BH.oM.Reflection;
using BH.oM.Reflection.Attributes;

namespace BH.Adapter.Filing
{
    public partial class FilingAdapter
    {
        /***************************************************/
        /**** Methods                                  *****/
        /***************************************************/

        [MultiOutput(0, "success", "List of booleans indicating whether the command succeeded for the individual items.")]
        [MultiOutput(1, "GlobalSuccess", "Bool indicating whether the command succeded for all the provided inputs.")]
        public override oM.Reflection.Output<List<object>, bool> Execute(IExecuteCommand command, ActionConfig actionConfig = null)
        {
            return RunCommand(command as dynamic);
        }

        /***************************************************/

        private oM.Reflection.Output<List<object>, bool> RunCommand(IMRCCommand command)
        {
            IEnumerable<Tuple<string, string>> oldAndTargetPaths =
                command.FullPath.Zip(command.TargetFullPath,
                (o, n) => new Tuple<string, string>(o.NormalisePath(), n.NormalisePath()));

            if (command is RenameCommand)
                return Move(oldAndTargetPaths, true);

            if (command is MoveCommand)
                return Move(oldAndTargetPaths, false);

            if (command is CopyCommand)
                return Copy(oldAndTargetPaths);

            return new Output<List<object>, bool>() { Item1 = null, Item2 = false };
        }

        private oM.Reflection.Output<List<object>, bool> Move(IEnumerable<Tuple<string, string>> oldAndTargetPaths, bool renameOnly)
        {
            var output = new Output<List<object>, bool>() { Item1 = new List<object>(), Item2 = false };

            foreach (var fullPaths in oldAndTargetPaths)
            {
                try
                {
                    string parent1 = System.IO.Directory.GetParent(fullPaths.Item1).FullName;
                    string parent2 = System.IO.Directory.GetParent(fullPaths.Item2).FullName;

                    if (renameOnly && parent1 != parent2)
                    {
                        BH.Engine.Reflection.Compute.RecordWarning($"Cannot rename `{fullPaths.Item1}`" +
                            $"because the target parent folder is different from the original.");

                        output.Item1.Add(false);
                    }
                    else
                    {
                        System.IO.File.Move(fullPaths.Item1, fullPaths.Item2);
                        output.Item1.Add(true);
                    }
                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordWarning(e.Message);
                }

                output.Item1.Add(false);
            }

            if (!output.Item1.Contains(false)) // all move/renames worked
                output.Item2 = true;

            return output;
        }

        private oM.Reflection.Output<List<object>, bool> Copy(IEnumerable<Tuple<string, string>> oldAndTargetPaths)
        {
            var output = new Output<List<object>, bool>() { Item1 = new List<object>(), Item2 = false };

            foreach (var fullPaths in oldAndTargetPaths)
            {
                try
                {
                    System.IO.File.Copy(fullPaths.Item1, fullPaths.Item2);
                    output.Item1.Add(true);
                }
                catch (Exception e)
                {
                    BH.Engine.Reflection.Compute.RecordWarning(e.Message);
                }
                output.Item1.Add(false);
            }

            if (!output.Item1.Contains(false)) // all copy worked
                output.Item2 = true;

            return output;
        }
    }
}
