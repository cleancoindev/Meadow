﻿using Meadow.Contract;
using Meadow.Core.EthTypes;
using Meadow.Core.Utils;
using Meadow.CoverageReport.Debugging;
using Meadow.EVM.Data_Types;
using Meadow.JsonRpc.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using System.Threading.Tasks;

namespace Meadow.UnitTestTemplate.Test
{
    [TestClass]
    public class DebuggerTests : ContractTest
    {
        VarAnalysisContract _contract;

        protected override async Task BeforeEach()
        {
            // Deploy our test contract
            _contract = await VarAnalysisContract.New(0, RpcClient);
        }

        [TestMethod]
        public async Task InConstructorTest()
        {
            ContractExecutionException exec = null;
            UInt256 testValue = 112233445566778899;
            try
            {
                await VarAnalysisContract.New(testValue, RpcClient);
            }
            catch (ContractExecutionException ex)
            {
                exec = ex;
            }

            // Obtain an execution trace and parse locals from the last point in it (where the exception occurred).
            var executionTrace = await RpcClient.GetExecutionTrace();
            ExecutionTraceAnalysis traceAnalysis = new ExecutionTraceAnalysis(executionTrace);

            // Obtain our local/state variables.
            var localVariables = traceAnalysis.GetLocalVariables();
            var stateVariables = traceAnalysis.GetStateVariables();

            // TODO: Verify variables.
            Assert.Inconclusive();
        }

        [TestMethod]
        public async Task InImmediateCall()
        {
            // Update our values 7 times.
            for (int i = 0; i < 7; i++)
            {
                await _contract.updateStateValues();
            }

            // Throw an exception in a function call (so we can simply check locals on latest execution point).
            await _contract.throwWithLocals(778899, 100).ExpectRevertTransaction();

            // Obtain an execution trace and parse locals from the last point in it (where the exception occurred).
            var executionTrace = await RpcClient.GetExecutionTrace();
            ExecutionTraceAnalysis traceAnalysis = new ExecutionTraceAnalysis(executionTrace);

            // Obtain our local/state variables.
            var localVariables = traceAnalysis.GetLocalVariables();
            var stateVariables = traceAnalysis.GetStateVariables();

            // TODO: Verify variables.
            Assert.Inconclusive();
        }

        [TestMethod]
        public async Task InDeeperCall()
        {
            // Update our values 7 times.
            for (int i = 0; i < 7; i++)
            {
                await _contract.updateStateValues();
            }

            // Throw an exception in a function call (so we can simply check locals on latest execution point).
            await _contract.indirectThrowWithLocals(778899, 100).ExpectRevertTransaction();

            // Obtain an execution trace and parse locals from the last point in it (where the exception occurred).
            var executionTrace = await RpcClient.GetExecutionTrace();
            ExecutionTraceAnalysis traceAnalysis = new ExecutionTraceAnalysis(executionTrace);

            // Obtain our local/state variables.
            var localVariables = traceAnalysis.GetLocalVariables();
            var stateVariables = traceAnalysis.GetStateVariables();

            // TODO: Verify variables.
            Assert.Inconclusive();
        }

        [TestMethod]
        public async Task TestBytesMemory()
        {
            // Create a byte array
            byte[] param1 = { 0x77, 0x88, 0x99, 0x88, 0x77, 0x00, 0x00, 0x00 };

            // Throw an exception in a function call (so we can simply check locals on latest execution point).
            await _contract.throwBytes(param1).ExpectRevertTransaction();

            // Obtain an execution trace and parse locals from the last point in it (where the exception occurred).
            var executionTrace = await RpcClient.GetExecutionTrace();
            ExecutionTraceAnalysis traceAnalysis = new ExecutionTraceAnalysis(executionTrace);

            // Obtain our local/state variables.
            var localVariables = traceAnalysis.GetLocalVariables();
            var stateVariables = traceAnalysis.GetStateVariables();

            // TODO: Verify variables.
            Assert.Inconclusive();
        }

        [TestMethod]
        public async Task TestArrayMemory()
        {
            // Update our values 7 times.
            for (int i = 0; i < 7; i++)
            {
                await _contract.updateStateValues();
            }

            // Throw an exception in a function call (so we can simply check locals on latest execution point).
            await _contract.throwArray().ExpectRevertTransaction();

            // Obtain an execution trace and parse locals from the last point in it (where the exception occurred).
            var executionTrace = await RpcClient.GetExecutionTrace();
            ExecutionTraceAnalysis traceAnalysis = new ExecutionTraceAnalysis(executionTrace);

            // Obtain our local/state variables.
            var localVariables = traceAnalysis.GetLocalVariables();
            var stateVariables = traceAnalysis.GetStateVariables();

            // TODO: Verify variables.
            Assert.Inconclusive();
        }

        [TestMethod]
        public async Task TestStructMemory()
        {
            // Update our values 7 times.
            for (int i = 0; i < 7; i++)
            {
                await _contract.updateStateValues();
            }

            // Throw an exception in a function call (so we can simply check locals on latest execution point).
            await _contract.throwStruct().ExpectRevertTransaction();

            // Obtain an execution trace and parse locals from the last point in it (where the exception occurred).
            var executionTrace = await RpcClient.GetExecutionTrace();
            ExecutionTraceAnalysis traceAnalysis = new ExecutionTraceAnalysis(executionTrace);

            // Obtain our local/state variables.
            var localVariables = traceAnalysis.GetLocalVariables();
            var stateVariables = traceAnalysis.GetStateVariables();

            // TODO: Verify variables.
            Assert.Inconclusive();
        }

        [TestMethod]
        public async Task TestMappings()
        {
            // Add all of our accounts to a mapping with the given index as the value.
            for (int i = 0; i < Accounts.Length; i++)
            {
                await _contract.updateMappings(Accounts[i], i);
            }

            // Throw an exception in a function call.
            await _contract.throwWithLocals(778899, 100).ExpectRevertTransaction();

            // Obtain an execution trace and parse locals from the last point in it (where the exception occurred).
            var executionTrace = await RpcClient.GetExecutionTrace();
            ExecutionTraceAnalysis traceAnalysis = new ExecutionTraceAnalysis(executionTrace);

            // Obtain our local/state variables.
            var localVariables = traceAnalysis.GetLocalVariables();
            var stateVariables = traceAnalysis.GetStateVariables();

            // TODO: Verify variables.
            Assert.Inconclusive();
        }
    }
}
